import MediaData from "./MediaData"
import TagData from "./TagData"

type UserData = {
    name: string,
    media: MediaData[],
    tags: TagData[]
}

export default UserData