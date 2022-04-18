import { useEffect, useState } from "react"
import Player from "../Components/Player"
import { useRoomHub } from "../Hooks/RoomHubProvider"
import MediaPlayedData from "../Types/MediaPlayedData"
import classes from "./Stage.module.scss"

const Stage = () => {
    const { connection } = useRoomHub()

    const [currentlyPlayingMedia, setCurrentlyPlayingMedia] = useState<MediaPlayedData | undefined>(undefined)

    const onPlayMedia = (mediaToPlay: MediaPlayedData) => {
        setCurrentlyPlayingMedia(mediaToPlay)
    }

    useEffect(() => {
        if (connection === undefined) return
        connection.on('PlayMedia', onPlayMedia)
    }, [connection, onPlayMedia])

    return (
        <div className={`d-flex ${classes.stretch}`}>
            {
                currentlyPlayingMedia && <Player currentlyPlaying={currentlyPlayingMedia} />
            }
        </div>
    )
}

export default Stage