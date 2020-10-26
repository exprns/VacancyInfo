import React from 'react';
import { Route } from 'react-router';
import { YMaps } from 'react-yandex-maps';
import { createMuiTheme, ThemeProvider } from '@material-ui/core/styles';
import { Home } from './pages/Home';

import './custom.css'


const theme = createMuiTheme({
    typography: {
        button: {
            textTransform: 'none',
            borderRadius: 0
        }
    }
});

export const App = () => (
    <ThemeProvider theme={ theme }>
        <YMaps query={ {
            lang: 'ru',
            quality: 2
        } }>
            <Route exact path='/' component={ Home } />
        </YMaps>
    </ThemeProvider>
);