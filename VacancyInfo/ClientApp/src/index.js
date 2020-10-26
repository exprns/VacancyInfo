import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { BrowserRouter } from 'react-router-dom';
import { App } from './App';
import registerServiceWorker from './registerServiceWorker';
import store from './store'

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

const render = () => ReactDOM.render(
    <Provider store={ store }>
        <BrowserRouter basename={ baseUrl }>
            <App />
        </BrowserRouter>
    </Provider>,
    rootElement);
render();
registerServiceWorker();



if (process.env.NODE_ENV === 'development' && module.hot) {
    module.hot.accept('./App', render)
}

