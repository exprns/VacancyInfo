import React from 'react';
import { Map as YMap } from 'react-yandex-maps';
import { MAP_COLORS } from '../../constants';
import { сustomizationRegions } from '../../utils';
import './Map.css'

export const Map = () => {
    const mapRef = React.createRef(null);
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
                    var regions = сustomizationRegions(result.features);
                    // Функция, которая раскрашивает регион и добавляет всех нераскрасшенных соседей в очередь на раскраску.
                    const paint = (iso) => {
                        const allowedColors = MAP_COLORS.slice();
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
                    mapRef.current.geoObjects.add(objectManager);
                    mapRef.current.geoObjects.events.add(['mouseenter', 'mouseleave'], function (e) {
                        console.log(e.get('type'), e.get('objectId'))
                        objectManager.objects.setObjectOptions(e.get('objectId'), {
                            fillOpacity: e.get('type') === 'mouseenter' ? 1 : 0.6
                        });
                    });
                });
        }
    };
    return <div className="map">
        <YMap
            instanceRef={ mapRef }
            defaultState={ { center: [65, 100], zoom: 2.5 } }
            onLoad={ handleOnLoad }
            // Подключаем модули регионов и ObjectManager
            modules={ ["borders", "ObjectManager", "pane.StaticPane", "templateLayoutFactory"] }
            width={ 1200 }
            height={ 500 }
        />
    </div>
}