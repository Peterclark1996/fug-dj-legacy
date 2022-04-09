import PlayerEnum from "./Enums/PlayerEnum";

export const BuildMediaResourceId = (player: PlayerEnum, code: string) => {
    switch (player) {
        case PlayerEnum.Youtube:
            return `y${code}`
        default:
            throw new Error("Unknown player type")
    }
}