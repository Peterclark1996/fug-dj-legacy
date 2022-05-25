import Player from "../../Components/Player"
import { useMediaQueue } from "../../Hooks/MediaQueueProvider"
import { useRoomHub } from "../../Hooks/RoomHubProvider"
import Character from "./Character"

const Stage = () => {
    const { currentlyPlaying } = useMediaQueue()
    const { connectedUsers } = useRoomHub()

    console.log("connectedUsers", connectedUsers)

    return (
        <div className="d-flex flex-grow-1 m-4 h-100">
            {
                currentlyPlaying && <Player currentlyPlaying={currentlyPlaying} />
            }
            {
                connectedUsers.map(user => <Character user={user} />)
            }
        </div>
    )
}

export default Stage