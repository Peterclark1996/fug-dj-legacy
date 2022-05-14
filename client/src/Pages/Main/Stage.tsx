import Player from "../../Components/Player"
import { useMediaQueue } from "../../Hooks/MediaQueueProvider"
import classes from "./Stage.module.scss"

const Stage = () => {
    const { currentlyPlaying } = useMediaQueue()

    return (
        <div className={`d-flex ${classes.stretch}`}>
            {
                currentlyPlaying && <Player currentlyPlaying={currentlyPlaying} />
            }
        </div>
    )
}

export default Stage