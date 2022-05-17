import { useState } from "react"
import Input from "../../Components/Input"
import Overlay from "../../Components/Overlay"
import StandardButton, { ButtonSize } from "../../Components/StandardButton"
import PageEnum from "../../Enums/PageEnum"
import Action, { ActionType } from "../../Reducer/Action"
import AppState from "../../Reducer/AppState"
import classes from "./ProfileScreen.module.scss"

type ProfileScreenProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const ProfileScreen = ({ state, dispatch }: ProfileScreenProps) => {
    const [usernameInput, setUsernameInput] = useState(state.userData?.name || "")

    const onCloseClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: PageEnum.Home })

    return (
        <Overlay onOutsideClick={onCloseClick}>
            <div className={`d-flex justify-content-between align-items-center p-2 rounded-top ${classes.background} ${classes.shadow}`}>
                <h1 className="ms-2 my-0">Profile</h1>
                <StandardButton className="m-2" iconClasses="fa-solid fa-xmark fa-2xl" toolTipText="Close" size={ButtonSize.LARGE} onClick={onCloseClick} />
            </div>
            <div className="m-4">
                <Input value={usernameInput} onChange={setUsernameInput} />
            </div>
        </Overlay>
    )
}

export default ProfileScreen