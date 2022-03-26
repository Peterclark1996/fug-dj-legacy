import PlayerEnum from "../Enums/PlayerEnum"
import { useRoomHub } from "../Hooks/RoomHubProvider"
import MediaData from "../Types/MediaData"
import TagData from "../Types/TagData"
import LinkButton from "./LinkButton"
import classes from "./Media.module.scss"
import StandardButton from "./StandardButton"
import Tag from "./Tag"

type MediaProps = {
    media: MediaData,
    userTags: TagData[]
}

const GetUrlForMedia = (media: MediaData) => {
    switch (media.player) {
        case 0:
            return "https://www.youtube.com/watch?v=" + media.code
        default:
            throw new Error("Unknown player type")
    }
}

const Media = ({ media, userTags }: MediaProps) => {
    const { connection, connectedRoomId } = useRoomHub()

    const onAddToQueueClick = () => {
        connection?.send('QueueMedia', connectedRoomId, media.player, media.code)
    }

    return (
        <div className={`d-flex flex-column m-1 p-1 user-select-none rounded ${classes.shadow}`}>
            <span className={classes.largeFont}>{media.name}</span>
            <div className="d-flex my-1">
                {
                    media.tags
                        .map(tag => userTags.find(userTag => userTag.id === tag))
                        .map((tagData, index) => {
                            return (
                                tagData && <Tag key={index} tag={tagData} />
                            )
                        })
                }
            </div>
            <div className="d-flex justify-content-between mt-auto">
                <StandardButton className="py-0 px-1 text-white" text="Add to Queue" onClick={onAddToQueueClick} />
                <LinkButton linkUrl={GetUrlForMedia(media)} text={PlayerEnum[media.player]} />
            </div>
        </div>
    )
}

export default Media