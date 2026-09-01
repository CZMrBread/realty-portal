// Collocated module of AdvertMap.razor. Leaflet itself is loaded globally from index.html.

const maps = new Map();
let nextId = 0;

// Czechia, which the portal serves, as the view when there is nothing to point at.
const defaultCenter = [49.8, 15.5];
const defaultZoom = 7;

export function create(element, markers) {
    const map = L.map(element);
    L.tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
        maxZoom: 19,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    const id = nextId++;
    maps.set(id, { map, layer: L.layerGroup().addTo(map) });
    setMarkers(id, markers);
    return id;
}

export function setMarkers(id, markers) {
    const state = maps.get(id);
    if (!state) {
        return;
    }

    state.layer.clearLayers();
    const positions = [];
    for (const marker of markers) {
        const pin = L.marker([marker.lat, marker.lng]).addTo(state.layer);
        if (marker.title) {
            // built as DOM rather than an HTML string, so the title cannot smuggle markup in
            const label = marker.url ? document.createElement("a") : document.createElement("span");
            label.textContent = marker.title;
            if (marker.url) {
                label.href = marker.url;
            }
            pin.bindPopup(label);
        }
        positions.push([marker.lat, marker.lng]);
    }

    if (positions.length === 1) {
        state.map.setView(positions[0], 15);
    } else if (positions.length > 1) {
        state.map.fitBounds(L.latLngBounds(positions), { padding: [30, 30] });
    } else {
        state.map.setView(defaultCenter, defaultZoom);
    }
}

export function dispose(id) {
    const state = maps.get(id);
    if (state) {
        state.map.remove();
        maps.delete(id);
    }
}
