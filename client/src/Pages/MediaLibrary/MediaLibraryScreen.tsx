import { useState } from "react"
import { useMutation, useQueryClient } from "react-query"
import Input from "../../Components/Input"
import Loading from "../../Components/Loading"
import Media from "./Media"
import Overlay from "../../Components/Overlay"
import StandardButton, { ButtonSize } from "../../Components/StandardButton"
import PageEnum from "../../Enums/PageEnum"
import PlayerEnum from "../../Enums/PlayerEnum"
import { useApi } from "../../Hooks/ApiProvider"
import Action, { ActionType } from "../../Reducer/Action"
import AppState from "../../Reducer/AppState"
import classes from "./MediaLibraryScreen.module.scss"

type LibraryScreenProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const MediaLibraryScreen = ({ state, dispatch }: LibraryScreenProps) => {
    const queryClient = useQueryClient()
    const { apiPost } = useApi()

    const [mediaUrl, setMediaUrl] = useState("")
    const [isMediaUrlValid, setMediaIsUrlValid] = useState(false)
    const [playerType, setPlayerType] = useState(undefined as PlayerEnum | undefined)
    const [playerCode, setPlayerCode] = useState("")

    const addMediaToUserMutation = useMutation(
        () => apiPost(`user/createmedia`, { Player: playerType, Code: playerCode }),
        {
            onSuccess: () => {
                queryClient.invalidateQueries(["user"])
            }
        }
    )

    const onChangeMediaUrl = (newUrl: string) => {
        setMediaUrl(newUrl)
        setMediaIsUrlValid(false)

        if (newUrl.includes("youtu.be/")) {
            setPlayerType(PlayerEnum.Youtube)
            const urlParts = newUrl.split("outu.be/")
            if (urlParts.length === 2) {
                trySetPlayerId(urlParts[1].split("&")[0])
            }
        } else if (newUrl.includes("www.youtube.com/")) {
            setPlayerType(PlayerEnum.Youtube)
            const urlParts = newUrl.split("outube.com/watch?v=")
            if (urlParts.length === 2) {
                trySetPlayerId(urlParts[1].split("&")[0])
            }
        }
    }

    const trySetPlayerId = (newPlayerCode: string) => {
        if (newPlayerCode !== removeUrlCharacters(newPlayerCode)) return
        setPlayerCode(newPlayerCode)
        setMediaIsUrlValid(true)
    }

    const removeUrlCharacters = (s: string) => s
        .replaceAll("=", "")
        .replaceAll("?", "")
        .replaceAll("/", "")
        .replaceAll("&", "")
        .replaceAll("\\", "")

    const onCloseClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: PageEnum.Home })

    return (
        <Overlay classname="d-flex flex-column" onOutsideClick={onCloseClick}>
            <div className={`d-flex justify-content-between align-items-center p-2 rounded-top ${classes.background} ${classes.shadow}`}>
                <h1 className="ms-2 my-0">Media Library</h1>
                <StandardButton className="m-2" iconClasses="fa-solid fa-xmark fa-2xl" toolTipText="Close" size={ButtonSize.LARGE} onClick={onCloseClick} />
            </div>
            <div className="d-flex flex-column flex-grow-1">
                <div className={`p-1 ${classes.grid}`}>
                    {
                        state.userData && state.userData.media.map((media, index) => <Media key={index} media={media} userTags={state.userData?.tags || []} />)
                    }
                </div>
            </div>
            <div className={`d-flex align-items-center p-2 rounded-bottom ${classes.background} ${classes.shadow}`}>
                <Input className="mx-2" placeholder="Paste a link here" value={mediaUrl} onChange={onChangeMediaUrl} isValid={mediaUrl === "" || isMediaUrlValid} />
                <Loading isLoading={addMediaToUserMutation.isLoading}>
                    <StandardButton className="m-2" text="Add" textClasses={classes.largeFont} iconClasses="fa-solid fa-plus fa-2xl" onClick={addMediaToUserMutation.mutate} />
                </Loading>
            </div>
        </Overlay>
    )
}

export default MediaLibraryScreen