import { useAuth0 } from '@auth0/auth0-react'
import { QueryClient, QueryClientProvider } from 'react-query'
import Loading from './Components/Loading'
import LoginScreen from './Screens/LoginScreen'
import MainScreen from './Screens/MainScreen'
import { ApiProvider } from './Hooks/ApiProvider'

const queryClient = new QueryClient()

const App = () => {
    const { user, isAuthenticated, isLoading } = useAuth0()

    return (
        <QueryClientProvider client={queryClient}>
            <ApiProvider>
                <Loading isLoading={isLoading}>
                    {isAuthenticated ? <MainScreen /> : <LoginScreen />}
                </ Loading>
            </ApiProvider>
        </QueryClientProvider>
    )
}

export default App