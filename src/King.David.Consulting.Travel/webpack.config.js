const path = require('path');
const webpack = require('webpack');
const CheckerPlugin = require('awesome-typescript-loader').CheckerPlugin;

module.exports = (env) => {
  const isDevBuild = !(env && env.prod);
  const clientBundleOutputDir = './wwwroot/dist';

  const clientBundleConfig = {
    stats: { modules: false },
    context: __dirname,
    resolve: { extensions: ['.js', '.ts'] },
    entry: { 'main-client': './ClientApp/main.ts' },
    output: {
      filename: '[name].js',
      publicPath: '/dist/', // Webpack dev middleware, if enabled, handles requests for this URL prefix
      path: path.join(__dirname, clientBundleOutputDir)
    },
    module: {
      rules: [
        { test: /\.ts$/, include: /ClientApp/, use: ['awesome-typescript-loader?silent=true', 'angular2-template-loader'] },
        { test: /\.html$/, use: 'html-loader?minimize=false' },
        { test: /\.css$/, use: ['to-string-loader', isDevBuild ? 'css-loader' : 'css-loader?minimize'] },
        { test: /\.(png|jpg|jpeg|gif|svg)$/, use: 'url-loader?limit=25000' }
      ]
    },
    plugins: [
      new CheckerPlugin(),
      new webpack.DllReferencePlugin({
        context: __dirname,
        manifest: require('./wwwroot/dist/vendor-manifest.json')
      })
    ].concat(isDevBuild ? [
      // Plugins that apply in development builds only
      new webpack.SourceMapDevToolPlugin({
        filename: '[file].map', // Remove this line if you prefer inline source maps
        moduleFilenameTemplate: path.relative(clientBundleOutputDir, '[resourcePath]') // Point sourcemap entries to the original file locations on disk
      })
    ] : [
        // Plugins that apply in production builds only
        new webpack.optimize.UglifyJsPlugin()
      ])
  };

  return [clientBundleConfig];
};