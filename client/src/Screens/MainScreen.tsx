import Action, { ActionType } from '../Reducer/Action'
import AppState from '../Reducer/AppState'
import Header from '../Panels/Header'
import RoomList from '../Panels/RoomList'
import classes from './Screen.module.scss'
import Stage from '../Panels/Stage'
import UserActions from '../Panels/UserActions'
import Page from '../Enums/Page'
import MediaLibraryScreen from './MediaLibraryScreen'
import { useQuery } from 'react-query'
import { useApi } from '../Hooks/ApiProvider'
import UserData from '../Types/UserData'
import { useAuth0 } from '@auth0/auth0-react'
import { useEffect } from 'react'

type MainScreenProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const MainScreen = ({ state, dispatch }: MainScreenProps) => {
    const { apiGet } = useApi()
    const { data: userData } = useQuery<UserData, Error>("user", () => apiGet(`user/get`))

    useEffect(() => {
        if (userData) {
            dispatch({ type: ActionType.USER_DATA_RETRIEVED, userData })
        }
    }, [userData])

    return (
        <div className={`d-flex flex-column ${classes.fullscreen} ${classes.background} ${classes.mainFont}`}>
            <Header />
            <div className="d-flex flex-grow-1">
                <div className="d-flex flex-column">
                    <RoomList />
                    <UserActions state={state} dispatch={dispatch} />
                </div>
                <Stage />
            </div>
            {
                state.selectedPage === Page.Library && <MediaLibraryScreen state={state} dispatch={dispatch} />
            }
        </div>
    )
}

export default MainScreen