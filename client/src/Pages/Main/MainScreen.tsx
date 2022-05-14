import Action, { ActionType } from '../../Reducer/Action'
import AppState from '../../Reducer/AppState'
import Header from './Header'
import RoomList from './RoomList'
import classes from '../Screen.module.scss'
import Stage from './Stage'
import UserActions from './UserActions'
import PageEnum from '../../Enums/PageEnum'
import MediaLibraryScreen from '../MediaLibrary/MediaLibraryScreen'
import { useQuery } from 'react-query'
import { useApi } from '../../Hooks/ApiProvider'
import UserData from '../../Types/UserData'
import { useEffect } from 'react'
import MediaQueue from './MediaQueue'

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
    }, [dispatch, userData])

    return (
        <div className={`d-flex flex-column ${classes.fullscreen} ${classes.background} ${classes.mainFont}`}>
            <Header />
            <div className="d-flex flex-grow-1">
                <div className="d-flex flex-column">
                    <RoomList />
                    <UserActions state={state} dispatch={dispatch} />
                </div>
                <Stage />
                <MediaQueue />
            </div>
            {
                state.selectedPage === PageEnum.Library && <MediaLibraryScreen state={state} dispatch={dispatch} />
            }
        </div>
    )
}

export default MainScreen