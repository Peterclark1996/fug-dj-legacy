import { memo } from "react"
import MediaPlayedData from "../Types/MediaPlayedData"

type PlayerProps = {
    currentlyPlaying: MediaPlayedData
}

const Player = ({ currentlyPlaying }: PlayerProps) => {
    return (<div>Playing: {currentlyPlaying.name}</div>)
}

export default memo(Player)