import StandardButton, { ButtonSize, ToolTipDirection } from '../Components/StandardButton'
import { useMediaQueue } from '../Hooks/MediaQueueProvider'
import classes from './MediaQueue.module.scss'

const MediaQueue = () => {
    const { queue, removeFromQueue, currentlyPlaying } = useMediaQueue()

    return (
        <div className={`d-flex flex-column align-items-center p-1 ${classes.responsiveWidth} ${classes.background} ${classes.shadow}`}>
            {
                currentlyPlaying &&
                <div className={`d-flex align-items-start w-100 p-1 mb-2 ${classes.shadow} ${classes.rounded} ${classes.mediumFont} ${classes.playing}`}>
                    {currentlyPlaying.name}
                </div>
            }

            {
                queue.map(media =>
                    <div
                        key={media.player + media.code}
                        className={`d-flex align-items-start w-100 p-1 mb-2 ${classes.shadow} ${classes.rounded} ${classes.mediumFont}`}
                    >
                        {media.name}
                        <StandardButton
                            className="ms-auto p-2"
                            iconClasses="fa-solid fa-trash-can"
                            toolTipText="Remove from queue"
                            toolTipDirection={ToolTipDirection.LEFT}
                            size={ButtonSize.SMALL}
                            onClick={() => removeFromQueue(media)}
                        />
                    </div>
                )
            }
        </div>
    )
}

export default MediaQueue