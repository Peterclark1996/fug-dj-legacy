import PageEnum from "../Enums/PageEnum"
import UserData from "../Types/UserData"

type AppState = {
    selectedPage: PageEnum,
    userData: UserData | null
}

export default AppState