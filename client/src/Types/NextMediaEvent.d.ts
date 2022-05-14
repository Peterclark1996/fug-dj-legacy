import MediaPlayedData from "./MediaPlayedData"

type NextMediaEvent = {
    justPlayed: MediaPlayedData | null,
    upNext: MediaPlayedData | null
}

export default NextMediaEvent