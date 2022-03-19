import { QueryClient, QueryClientProvider } from 'react-query'
import Main from './Screens/Main'

const queryClient = new QueryClient()

const App = () => {
    return (
        <QueryClientProvider client={queryClient}>
            <Main />
        </QueryClientProvider>
    )
}

export default App