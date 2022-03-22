import Page from "../Enums/Page"
import UserData from "../Types/UserData"

type AppState = {
    selectedPage: Page,
    userData: UserData | null
}

export default AppState