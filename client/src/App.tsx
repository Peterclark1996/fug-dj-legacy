import { useAuth0 } from '@auth0/auth0-react'
import { QueryClient, QueryClientProvider } from 'react-query'
import Loading from './Components/Loading'
import LoginScreen from './Pages/Login/LoginScreen'
import MainScreen from './Pages/Main/MainScreen'
import { ApiProvider } from './Hooks/ApiProvider'
import { useReducer } from 'react'
import Reducer from './Reducer/Reducer'
import PageEnum from './Enums/PageEnum'
import { RoomHubProvider } from './Hooks/RoomHubProvider'
import { MediaQueueProvider } from './Hooks/MediaQueueProvider'

const queryClient = new QueryClient()

const App = () => {
    const { isAuthenticated, isLoading } = useAuth0()

    const [state, dispatch] = useReducer(Reducer, {
        selectedPage: PageEnum.Home,
        userData: null
    })

    return (
        <QueryClientProvider client={queryClient}>
            <ApiProvider>
                <RoomHubProvider>
                    <MediaQueueProvider>
                        <Loading isLoading={isLoading}>
                            {isAuthenticated ? <MainScreen state={state} dispatch={dispatch} /> : <LoginScreen />}
                        </ Loading>
                    </MediaQueueProvider>
                </RoomHubProvider>
            </ApiProvider>
        </QueryClientProvider>
    )
}

export default App