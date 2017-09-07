using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using King.David.Consulting.Travel.Web.Domain;
using King.David.Consulting.Travel.Web.Features.Users;
using MediatR;

namespace King.David.Consulting.Travel.Web.Infrastructure
{
    public class DbInitializer
    {
        private AppDbContext _ctx;
        private IMediator _mediator;

        public DbInitializer(AppDbContext ctx, IMediator mediator)
        {
            _ctx = ctx;
            _mediator = mediator;
        }

        public async Task Seed()
        {
            //_ctx.Database.EnsureCreated();

            Assembly assembly = typeof(DbInitializer).GetTypeInfo().Assembly;

            if (!_ctx.States.Any())
            {

                string resourceName = "King.David.Consulting.Travel.Web.data.State.csv";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;
                        var records = csvReader.GetRecords<State>().ToArray();
                        _ctx.States.AddRange(records);
                        await _ctx.SaveChangesAsync();
                    }
                }

            }

            if (!_ctx.Cities.Any())
            {

                string resourceName = "King.David.Consulting.Travel.Web.data.City.csv";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;
                        var records = csvReader.GetRecords<City>().ToArray();
                        _ctx.Cities.AddRange(records);
                        await _ctx.SaveChangesAsync();
                    }
                }

            }

            if (!_ctx.Users.Any())
            {
                string resourceName = "King.David.Consulting.Travel.Web.data.User.csv";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader);
                        csvReader.Configuration.WillThrowOnMissingField = false;
                        var records = csvReader.GetRecords<Domain.User>().ToArray();

                        //get each user and process it one in a time
                        var count = 1;
                        foreach (var user in records)
                        {
                            #region Create an user
                            var userName = $"user{count}";
                            var userPassword = $"P@ssword{count}";
                            var userEmail = $"{user.FirstName.ToLower().Trim()}.{user.LastName.ToLower().Trim()}@web.com";

                            var createCmd = new Create.Command();

                            createCmd.User = new Create.UserData()
                            {
                                Email = userEmail,
                                Password = userPassword,
                                Username = userName
                            };

                            await _mediator.Send(createCmd);
                            #endregion

                            #region Update Users and Visits
                            var addUser = _ctx.Users.FirstOrDefault(x => x.Username == createCmd.User.Username);
                            if (addUser != null)
                            {
                                addUser.FirstName = user.FirstName;
                                addUser.LastName = user.LastName;
                                _ctx.Update(addUser);
                                await _ctx.SaveChangesAsync();

                                var cities = _ctx.Cities.Where(x => x.Name.StartsWith("A")).ToList();

                                var visits = new List<UserVisit>()
                                                {
                                                    new UserVisit()
                                                    {
                                                        UserId = addUser.UserId,
                                                        CityId = cities[0].CityId,
                                                        StateId = cities[0].StateId
                                                    },
                                                    new UserVisit()
                                                    {
                                                        UserId = addUser.UserId,
                                                         CityId = cities[1].CityId,
                                                        StateId = cities[1].StateId
                                                    },
                                                    new UserVisit()
                                                    {
                                                        UserId = addUser.UserId,
                                                         CityId = cities[2].CityId,
                                                        StateId = cities[2].StateId
                                                    }
                                               };

                                await _ctx.AddRangeAsync(visits);

                                await _ctx.SaveChangesAsync();
                            }

                            #endregion
                               
                            count += 1;
                        }
                        
                       
                    }
                }

            }
        }
    }
}