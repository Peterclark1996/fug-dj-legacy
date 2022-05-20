import { useState } from "react"
import { useMutation, useQueryClient } from "react-query"
import Input from "../../Components/Input"
import Overlay, { OverlaySize } from "../../Components/Overlay"
import StandardButton, { ButtonSize } from "../../Components/StandardButton"
import { Endpoint, Resource } from "../../Constants"
import PageEnum from "../../Enums/PageEnum"
import { useApi } from "../../Hooks/ApiProvider"
import Action, { ActionType } from "../../Reducer/Action"
import AppState from "../../Reducer/AppState"
import classes from "./ProfileScreen.module.scss"

type ProfileScreenProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const ProfileScreen = ({ state, dispatch }: ProfileScreenProps) => {
    const queryClient = useQueryClient()
    const { apiPatch } = useApi()

    const [isEditingUsername, setIsEditingUsername] = useState(false)
    const [usernameInput, setUsernameInput] = useState(state.userData?.name || "")

    const onCloseClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: PageEnum.Home })

    const updateUserMutation = useMutation(
        (username: string) => apiPatch(Endpoint.USER, { name: username }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries([Resource.USER])
            }
        }
    )

    const onSaveUsernameClick = () => {
        if (!isUsernameValid) return

        updateUserMutation.mutate(usernameInput)

        setIsEditingUsername(false)
    }

    const isUsernameValid = usernameInput.length !== 0 && usernameInput.length < 20

    return (
        <Overlay size={OverlaySize.THIN} onOutsideClick={onCloseClick}>
            <div className={`d-flex justify-content-between align-items-center p-2 rounded-top ${classes.background} ${classes.shadow}`}>
                <h1 className="ms-2 my-0">Profile</h1>
                <StandardButton className="m-2" iconClasses="fa-solid fa-xmark fa-2xl" toolTipText="Close" size={ButtonSize.LARGE} onClick={onCloseClick} />
            </div>
            <div className="d-flex align-items-center m-4">
                {
                    <Input value={usernameInput} onChange={setUsernameInput} isEnabled={isEditingUsername} />
                }
                <div style={{ "width": "20px" }}>
                    {
                        isEditingUsername ?
                            <i className="fa-solid fa-check fa-sm ms-2 text-success" role="button" onClick={onSaveUsernameClick} /> :
                            <i className="fa-solid fa-pencil fa-2xs ms-2" role="button" onClick={() => setIsEditingUsername(true)} />
                    }
                </div>
            </div>
        </Overlay>
    )
}

export default ProfileScreen