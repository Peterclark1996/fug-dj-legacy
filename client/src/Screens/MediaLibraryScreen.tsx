import Media from "../Components/Media"
import Overlay from "../Components/Overlay"
import StandardButton from "../Components/StandardButton"
import PageEnum from "../Enums/PageEnum"
import Action, { ActionType } from "../Reducer/Action"
import AppState from "../Reducer/AppState"
import classes from "./MediaLibraryScreen.module.scss"

type LibraryScreenProps = {
    state: AppState,
    dispatch: React.Dispatch<Action>
}

const MediaLibraryScreen = ({ state, dispatch }: LibraryScreenProps) => {
    const onCloseClick = () => dispatch({ type: ActionType.SELECTED_PAGE_UPDATED, updatedPage: PageEnum.Home })

    return (
        <Overlay onOutsideClick={onCloseClick}>
            <div className={`d-flex flex-grow-1 justify-content-between align-items-center p-2 rounded-top ${classes.background} ${classes.shadow}`}>
                <h1 className="ms-2 my-0">Media Library</h1>
                <StandardButton className="m-2" iconClasses="fa-solid fa-xmark fa-2xl" toolTipText="Close" isFixedSize={true} onClick={onCloseClick} />
            </div>
            <div className={`p-1 ${classes.grid}`}>
                {
                    state.userData && state.userData.media.map((media, index) => <Media key={index} media={media} userTags={state.userData?.tags || []} />)
                }
            </div>
        </Overlay>
    )
}

export default MediaLibraryScreen