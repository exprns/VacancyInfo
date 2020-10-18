import React, { useState } from 'react';
import { TextField, Button } from '@material-ui/core'
import { Map } from 'react-yandex-maps';
import './Home.css'

export const Home = () => {
    const mapRef = React.createRef(null);
    const [text, setText] = useState('');

    const getFoo = () => {
        fetch(`api/Vacancy/Vacancies?name=${text}`).then(res => console.log(res)).catch(err => console.error(err))
    }

    const handleOnLoad = (ymaps) => {
        if (mapRef && mapRef.current) {
            let objectManager = new ymaps.ObjectManager();
            let pane = new ymaps.pane.StaticPane(mapRef.current, {
                zIndex: 100, css: {
                    width: '100%', height: '100%', backgroundColor: '#FFFFFF'
                }
            });

            mapRef.current.panes.append('white', pane)
            ymaps.borders
                .load("RU", {
                    lang: "ru",
                    quality: 2
                })
                .then(function (result) {
                    // Очередь раскраски.
                    var queue = [];
                    // Создадим объект regions, где ключи это ISO код региона.
                    var regions = result.features.reduce(function (acc, feature) {
                        // Добавим ISO код региона в качестве feature.id для objectManager.
                        var iso = feature.properties.iso3166;
                        feature.id = iso;
                        // Добавим опции региона по умолчанию.
                        feature.options = {
                            fillOpacity: 0.6,
                            strokeColor: "#FFF",
                            strokeOpacity: 0.8
                        };
                        acc[iso] = feature;
                        return acc;
                    }, {});
                    // Функция, которая раскрашивает регион и добавляет всех нераскрасшенных соседей в очередь на раскраску.
                    const paint = (iso) => {
                        const allowedColors = COLORS.slice();
                        // Получим ссылку на раскрашиваемый регион и на его соседей.
                        const region = regions[iso];
                        const neighbors = region.properties.neighbors;
                        // Если у региона есть опция fillColor, значит мы его уже раскрасили.
                        if (region.options.fillColor) {
                            return;
                        }
                        // Если у региона есть соседи, то нужно проверить, какие цвета уже заняты.
                        if (neighbors.length !== 0) {
                            neighbors.forEach(function (neighbor) {
                                var fillColor = regions[neighbor].options.fillColor;
                                // Если регион раскрашен, то исключаем его цвет.
                                if (fillColor) {
                                    var index = allowedColors.indexOf(fillColor);
                                    if (index !== -1) {
                                        allowedColors.splice(index, 1);
                                    }
                                    // Если регион не раскрашен, то добавляем его в очередь на раскраску.
                                } else if (queue.indexOf(neighbor) === -1) {
                                    queue.push(neighbor);
                                }
                            });
                        }
                        // Раскрасим регион в первый доступный цвет.
                        region.options.fillColor = allowedColors[0];
                    }
                    for (const iso in regions) {
                        // Если регион не раскрашен, добавим его в очередь на раскраску.
                        if (!regions[iso].options.fillColor) {
                            queue.push(iso);
                        }
                        // Раскрасим все регионы из очереди.
                        while (queue.length > 0) {
                            paint(queue.shift());
                        }
                    }
                    // Добавим регионы на карту.
                    result.features = [];
                    for (const reg in regions) {
                        result.features.push(regions[reg]);
                    }
                    objectManager.add(result);
                    /*   var polylabel = new ymaps.polylabel.create(mapRef.current, objectManager); */
                    mapRef.current.geoObjects.add(objectManager);
                    mapRef.current.geoObjects.events.add(['mouseenter', 'mouseleave'], function (e) {
                        console.log(e.get('type'), e.get('objectId'))
                        objectManager.objects.setObjectOptions(e.get('objectId'), {
                            fillOpacity: e.get('type') == 'mouseenter' ? 1 : 0.6
                        });
                    });
                });
        }
    };

    const handleText = (e) => setText(e.target.value)
    const COLORS = ["#F0F075", "#FB6C3F", "#3D4C76", "#49C0B5"];
    return (
        <div className="home">
            <div className="row">
                <TextField label="Поле для Масима" onChange={ handleText } />
                <Button
                    color="primary"
                    onClick={ getFoo }
                >
                    Кнопочка
            </Button>
            </div>
            <div className="map">
                <Map
                    instanceRef={ mapRef }
                    defaultState={ { center: [65, 100], zoom: 2.5 } }
                    onLoad={ handleOnLoad }
                    // Подключаем модули регионов и ObjectManager
                    modules={ ["borders", "ObjectManager", "pane.StaticPane", "templateLayoutFactory"] }
                    width={ 1200 }
                    height={ 500 }
                />
            </div>
        </div>
    );
}
