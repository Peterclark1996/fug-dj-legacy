import Header from '../Panels/Header'
import RoomList from '../Panels/RoomList'
import Stage from '../Panels/Stage'
import classes from './Main.module.scss'

const Main = () => {
    return (
        <div className={`h-100 w-100 d-flex flex-column ${classes.fullscreen} ${classes.background} ${classes.mainFont}`}>
            <Header />
            <div className="d-flex flex-grow-1">
                <RoomList />
                <Stage />
            </div>
        </div>
    )
}

export default Main