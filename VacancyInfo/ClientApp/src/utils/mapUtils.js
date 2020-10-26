export const сustomizationRegions = (features) => (features.reduce(function (acc, feature) {
    console.log(features)
    // Добавим ISO код региона в качестве feature.id для objectManager.
    var iso = feature.properties.iso3166;
    feature.properties.iconCaption = 100 + Math.random(0, 1000)
    feature.id = iso;
    // Добавим опции региона по умолчанию.
    feature.options = {
        fillOpacity: 0.6,
        strokeColor: "#FFF",
        strokeOpacity: 0.8
    };
    acc[iso] = feature;
    return acc;
}, {}));