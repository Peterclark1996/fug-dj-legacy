import Header from '../Panels/Header'
import RoomList from '../Panels/RoomList'
import Stage from '../Panels/Stage'
import UserActions from '../Panels/UserActions'
import classes from './Screen.module.scss'

const MainScreen = () => {
    return (
        <div className={`d-flex flex-column ${classes.fullscreen} ${classes.background} ${classes.mainFont}`}>
            <Header />
            <div className="d-flex flex-grow-1">
                <div className="d-flex flex-column">
                    <RoomList />
                    <UserActions />
                </div>
                <Stage />
            </div>
        </div>
    )
}

export default MainScreen