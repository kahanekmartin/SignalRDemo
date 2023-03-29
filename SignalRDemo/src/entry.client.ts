import React from 'react'
import ReactDOM from 'react-dom'
/* @ts-ignore */
import global from 'global'
import { components } from './components'

/* @ts-ignore */
import './index.css'

global.React = React;
global.ReactDOM = ReactDOM;
global.Components = components;

if (!global.Components) global.Components = {}

Object.assign(global.Components, {
    components
})