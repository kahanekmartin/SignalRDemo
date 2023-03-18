const path = require("path");

/** @type {import('webpack').Configuration} */
const clientConfig = {
  entry: "./entry.client.ts",
  mode: "development",
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
    ],
  },
  output: {
    filename: "client.js",
    path: path.resolve(__dirname, "../wwwroot/dist"),
    publicPath: "/dist/",
  },
  resolve: {
    extensions: [".tsx", ".ts", ".js"],
  },
  target: "browserslist",
};

/** @type {import('webpack').Configuration} */
const serverConfig = {
  entry: "./entry.server.ts",
  mode: "development",
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
    ],
  },
  output: {
    filename: "server.js",
    path: path.resolve(__dirname, "../wwwroot/dist"),
    publicPath: "/dist/",
  },
  resolve: {
    extensions: [".tsx", ".ts", ".js"],
  },
  target: "node",
};

module.exports = [clientConfig, serverConfig];