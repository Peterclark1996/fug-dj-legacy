import { useAuth0 } from '@auth0/auth0-react'
import { createContext, useContext } from 'react'
import axios from 'axios'

const getApiUrl = () => {
    if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
        return "https://localhost:7177/api/"
    }
    return "https://localhost:7177/api/"
}

interface ApiContextInterface {
    apiGet: (url: string) => Promise<any>,
    apiPost: (url: string, data: any) => Promise<any>
}

const ApiContext = createContext<ApiContextInterface>({
    apiGet: () => Promise.resolve({}),
    apiPost: () => Promise.resolve({})
})

export const ApiProvider = (props: React.PropsWithChildren<{}>) => {
    const { getAccessTokenSilently } = useAuth0()

    const apiGet = (url: string) =>
        getAccessTokenSilently()
            .then(token => axios.get(`${getApiUrl()}${url}`, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const apiPost = (url: string, data: any) =>
        getAccessTokenSilently()
            .then(token => axios.post(`${getApiUrl()}${url}`, data, { headers: { Authorization: `Bearer ${token}` } }))
            .then(res => res.data)

    const value = {
        apiGet,
        apiPost
    }

    return <ApiContext.Provider value={value} {...props} />
}

export const useApi = () => useContext(ApiContext)