const path = require('path');
const webpack = require("webpack");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
module.exports = {
    mode: 'none',
    entry: {
        app: './src/index.tsx',
    },
    output: {
        filename: 'main.js',
        publicPath: "",
        path: path.resolve(__dirname, 'wwwroot/js')
    },
    resolve: {
        extensions: [".ts", ".tsx"]
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            },
        ]
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new CleanWebpackPlugin()
    ]
};