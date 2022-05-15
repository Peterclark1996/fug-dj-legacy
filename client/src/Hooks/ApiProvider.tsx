import { useAuth0 } from '@auth0/auth0-react'
import { createContext, useContext } from 'react'
import axios from 'axios'
import { ApiUrl } from '../Constants'

const getApiUrl = () => {
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        return ApiUrl.DEV
    }
    return ApiUrl.PROD
}

interface ApiContextInterface {
    apiGet: (url: string) => Promise<unknown>,
    apiPost: (url: string, data?: unknown | undefined) => Promise<unknown>,
    apiPut: (url: string, data: unknown) => Promise<unknown>,
    apiPatch: (url: string, data: unknown) => Promise<unknown>,
    apiDelete: (url: string) => Promise<unknown>
}

const ApiContext = createContext<ApiContextInterface>({
    apiGet: () => Promise.resolve({}),
    apiPost: () => Promise.resolve({}),
    apiPut: () => Promise.resolve({}),
    apiPatch: () => Promise.resolve({}),
    apiDelete: () => Promise.resolve({})
})

export const ApiProvider = (props: React.PropsWithChildren<unknown>) => {
    const { getAccessTokenSilently } = useAuth0()

    const apiGet = (url: string) =>
        getAccessTokenSilently()
            .then(token => axios.get(`${getApiUrl()}${url}`, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const apiPost = (url: string, data?: unknown) =>
        getAccessTokenSilently()
            .then(token => axios.post(`${getApiUrl()}${url}`, data, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const apiPut = (url: string, data: unknown) =>
        getAccessTokenSilently()
            .then(token => axios.put(`${getApiUrl()}${url}`, data, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const apiPatch = (url: string, data: unknown) =>
        getAccessTokenSilently()
            .then(token => axios.patch(`${getApiUrl()}${url}`, data, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const apiDelete = (url: string) =>
        getAccessTokenSilently()
            .then(token => axios.delete(`${getApiUrl()}${url}`, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const value = {
        apiGet,
        apiPost,
        apiPut,
        apiPatch,
        apiDelete
    }

    return <ApiContext.Provider value={value} {...props} />
}

export const useApi = () => useContext(ApiContext)