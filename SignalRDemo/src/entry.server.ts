import React from 'react';
import ReactDOM from 'react-dom';
import ReactDOMServer from 'react-dom/server';
// @ts-ignore
import global from 'global'
import { components } from "./components";

/* @ts-ignore */
import './index.css'

global.React = React;
global.ReactDOM = ReactDOM;
global.ReactDOMServer = ReactDOMServer;
global.Components = components;