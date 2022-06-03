import Player from "./Player"
import { useMediaQueue } from "../../Hooks/MediaQueueProvider"
import { useRoomHub } from "../../Hooks/RoomHubProvider"
import Character from "./Character"

const Stage = () => {
    const { currentlyPlaying } = useMediaQueue()
    const { connectedUsers } = useRoomHub()

    return (
        <div className="d-flex flex-column flex-grow-1 m-4 h-100 align-items-center">
            <Player currentlyPlaying={currentlyPlaying} />
            <div className="d-flex h-100 w-100">
                {
                    connectedUsers.map(user => <Character user={user} />)
                }
            </div>
        </div>
    )
}

export default Stage