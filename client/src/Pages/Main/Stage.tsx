import Player from "../../Components/Player"
import { useMediaQueue } from "../../Hooks/MediaQueueProvider"
import { useRoomHub } from "../../Hooks/RoomHubProvider"
import classes from "./Stage.module.scss"

const Stage = () => {
    const { currentlyPlaying } = useMediaQueue()
    const { connectedUsers } = useRoomHub()

    console.log("connectedUsers", connectedUsers)

    return (
        <div className={`d-flex ${classes.stretch}`}>
            {
                currentlyPlaying && <Player currentlyPlaying={currentlyPlaying} />
            }
        </div>
    )
}

export default Stage