import Overlay from "../Components/Overlay"
import StandardButton from "../Components/StandardButton"
import Page from "../Enums/Page"
import Action, { ActionType } from "../Reducer/Action"
import AppState from "../Reducer/AppState"
import classes from "./MediaLibraryScreen.module.scss"

type LibraryScreenProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const MediaLibraryScreen = ({ state, dispatch }: LibraryScreenProps) => {
    const onCloseClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: Page.Home })

    return (
        <Overlay onOutsideClick={onCloseClick}>
            <div className={`d-flex flex-grow-1 justify-content-between p-2 rounded-top ${classes.background} ${classes.shadow}`}>
                <h1 className="ms-2">Media Library</h1>
                <StandardButton className="m-2" iconClasses="fa-solid fa-xmark fa-2xl" toolTipText="Close" isFixedSize={true} onClick={onCloseClick} />
            </div>
            <div className="d-flex flex-column">
                {
                    state.userData && state.userData.media.map((media, index) => <span key={index}>{media.name}</span>)
                }
            </div>
        </Overlay>
    )
}

export default MediaLibraryScreen