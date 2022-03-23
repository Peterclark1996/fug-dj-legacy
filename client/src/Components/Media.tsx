import PlayerEnum from "../Enums/PlayerEnum"
import MediaData from "../Types/MediaData"
import TagData from "../Types/TagData"
import LinkButton from "./LinkButton"
import classes from "./Media.module.scss"
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
            <div className="d-flex justify-content-end mt-auto">
                <LinkButton linkUrl={GetUrlForMedia(media)} text={PlayerEnum[media.player]} />
            </div>
        </div>
    )
}

export default Media