import { useAuth0 } from '@auth0/auth0-react'
import { QueryClient, QueryClientProvider } from 'react-query'
import Loading from './Components/Loading'
import LoginScreen from './Screens/LoginScreen'
import MainScreen from './Screens/MainScreen'
import { ApiProvider } from './Hooks/ApiProvider'
import { useReducer } from 'react'
import Reducer from './Reducer/Reducer'
import Page from './Enums/Page'

const queryClient = new QueryClient()

const App = () => {
    const { isAuthenticated, isLoading } = useAuth0()

    const [state, dispatch] = useReducer(Reducer, {
        selectedPage: Page.Home,
        userData: null
    })

    return (
        <QueryClientProvider client={queryClient}>
            <ApiProvider>
                <Loading isLoading={isLoading}>
                    {isAuthenticated ? <MainScreen state={state} dispatch={dispatch} /> : <LoginScreen />}
                </ Loading>
            </ApiProvider>
        </QueryClientProvider>
    )
}

export default App