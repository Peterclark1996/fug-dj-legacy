import { useAuth0 } from '@auth0/auth0-react'
import { QueryClient, QueryClientProvider } from 'react-query'
import Loading from './Components/Loading'
import LoginScreen from './Screens/LoginScreen'
import MainScreen from './Screens/MainScreen'

const queryClient = new QueryClient()

const App = () => {
    const { user, isAuthenticated, isLoading } = useAuth0()

    return (
        <QueryClientProvider client={queryClient}>
            <Loading isLoading={isLoading}>
                {isAuthenticated ? <MainScreen /> : <LoginScreen />}
            </ Loading>
        </QueryClientProvider>
    )
}

export default App