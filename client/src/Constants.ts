export enum Endpoint {
    GET_USER = "user/get",
    GET_ALL_ROOMS = "rooms/getall",
    POST_CREATE_MEDIA_TAG = "user/createmediatag",
    POST_CREATE_MEDIA = "user/createmedia",
    POST_DELETE_MEDIA = "user/deletemedia",
    PATCH_UPDATE_MEDIA = "user/updatemedia"
}

export enum Resource {
    USER = "user"
}

export enum ApiUrl {
    DEV = "https://localhost:7177/api/",
    PROD = "https://localhost:7177/api/"
}

export enum HubUrl {
    DEV = "https://localhost:7177/hub/room",
    PROD = "https://localhost:7177/hub/room"
}

export const getYoutubeUrl = (code: string) => `https://www.youtube.com/watch?v=${code}`