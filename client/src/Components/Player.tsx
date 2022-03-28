import { memo } from "react"
import MediaPlayedData from "../Types/MediaPlayedData"

type PlayerProps = {
    currentlyPlaying: MediaPlayedData
}

const Player = ({ currentlyPlaying }: PlayerProps) => {
    console.log(currentlyPlaying.name)

    return (<div>Playing: {currentlyPlaying.name}</div>)
}

export default memo(Player)