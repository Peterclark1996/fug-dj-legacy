import MediaData from "./MediaData.d"
import TagData from "./TagData"

type UserData = {
    name: string,
    media: MediaData[],
    tags: TagData[]
}

export default UserData