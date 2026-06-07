/**
 * DocDocGo API client — web UI calls REST API using the login cookie session.
 */
const DocDocGoApi = (function () {
    const baseUrl = '/api';

    async function request(method, path, body) {
        const options = {
            method: method,
            credentials: 'include',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        };

        if (body !== undefined && body !== null) {
            options.body = JSON.stringify(body);
        }

        const response = await fetch(baseUrl + path, options);

        if (response.status === 401) {
            window.location.href = '/Account/Login';
            throw new Error('Unauthorized');
        }

        if (response.status === 204) {
            return null;
        }

        const text = await response.text();
        const data = text ? JSON.parse(text) : null;

        if (!response.ok) {
            const message = data?.title || data?.message || data || response.statusText;
            throw new Error(typeof message === 'string' ? message : 'Request failed');
        }

        return data;
    }

    return {
        get: (path) => request('GET', path),
        post: (path, body) => request('POST', path, body),
        put: (path, body) => request('PUT', path, body),
        delete: (path) => request('DELETE', path),

        patients: {
            getAll: () => request('GET', '/patients'),
            getById: (id) => request('GET', '/patients/' + id),
            create: (data) => request('POST', '/patients', data),
            update: (id, data) => request('PUT', '/patients/' + id, data),
            delete: (id) => request('DELETE', '/patients/' + id)
        },
        appointments: {
            getAll: () => request('GET', '/appointments'),
            getById: (id) => request('GET', '/appointments/' + id),
            create: (data) => request('POST', '/appointments', data),
            update: (id, data) => request('PUT', '/appointments/' + id, data),
            delete: (id) => request('DELETE', '/appointments/' + id)
        },
        prescriptions: {
            getAll: () => request('GET', '/prescriptions'),
            getById: (id) => request('GET', '/prescriptions/' + id),
            create: (data) => request('POST', '/prescriptions', data),
            update: (id, data) => request('PUT', '/prescriptions/' + id, data),
            delete: (id) => request('DELETE', '/prescriptions/' + id)
        },
        reports: {
            getAll: () => request('GET', '/reports'),
            getById: (id) => request('GET', '/reports/' + id)
        }
    };
})();
