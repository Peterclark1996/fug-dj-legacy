import Action, { ActionType } from "./Action"
import AppState from "./AppState"

const Reducer = (state: AppState, action: Action): AppState => {
    switch (action.type) {
        case ActionType.SELECTED_PAGE_UPDATED:
            return {
                ...state,
                selectedPage: action.updatedPage
            }
        case ActionType.USER_DATA_RETRIEVED:
            return {
                ...state,
                userData: action.userData
            }
    }
}

export default Reducer