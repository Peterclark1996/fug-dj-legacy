import PlayerEnum from "./Enums/PlayerEnum"

export enum Endpoint {
    USER = "user",
    GET_USER = "user/get",
    GET_ALL_ROOMS = "rooms/getall",
    POST_CREATE_MEDIA_TAG = "user/createmediatag"
}

export const getMediaUrl = (player: PlayerEnum, code: string) => `media/${getMediaId(player, code)}`

const getMediaId = (player: PlayerEnum, code: string) => {
    switch (player) {
        case PlayerEnum.Youtube:
            return `y${code}`
        default:
            throw new Error("Unknown player type")
    }
}

export enum Resource {
    USER = "user",
    ROOMS = "rooms"
}

export enum ApiUrl {
    DEV = "https://localhost:7177/api/",
    PROD = "https://fug-dj.herokuapp.com/api/"
}

export enum HubUrl {
    DEV = "https://localhost:7177/hub/room",
    PROD = "https://fug-dj.herokuapp.com/hub/room"
}

export const getYoutubeUrl = (code: string) => `https://www.youtube.com/watch?v=${code}`