import React from 'react';
import { Route } from 'react-router';
import { YMaps } from 'react-yandex-maps';
import { Layout } from './components/Layout';
import { Home } from './components/Home';

import './custom.css'

export const App = () => (
    <Layout>
        <YMaps query={ {
            lang: 'ru',
            quality: 2
        } }>
            <Route exact path='/' component={ Home } />
        </YMaps>
    </Layout>
);