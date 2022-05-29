import { memo } from "react"
import MediaPlayedData from "../../Types/MediaPlayedData"
import YoutubePlayer from "./YoutubePlayer"
import classes from "./Player.module.scss"

type PlayerProps = {
    currentlyPlaying: MediaPlayedData | undefined
}

const Player = ({ currentlyPlaying }: PlayerProps) => {
    const getPlayer = () => {
        switch (currentlyPlaying?.player) {
            case 0:
                return <YoutubePlayer videoCode={currentlyPlaying.code} />
            default:
                return <div>No player for this media type</div>
        }
    }

    return (
        <div className={classes.fixedHeight}>
            {currentlyPlaying && getPlayer()}
        </div>
    )
}

export default memo(Player)