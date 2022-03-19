import classes from './RoomButton.module.scss'

type RoomButtonProps = {
    roomName: string
    selected: boolean
    onClick: () => void
}

const RoomButton = ({ roomName, selected, onClick }: RoomButtonProps) => {
    return (
        <div role='button' onClick={onClick} className={`d-flex justify-content-center align-items-center user-select-none mx-1 my-2 p-1 ${selected ? classes.selected : classes.notSelected} ${classes.square} ${classes.rounded} ${classes.background} ${classes.toolTip}`}>
            {roomName[0].toUpperCase()}
            <span className={`text-nowrap px-1 rounded bg-dark ${classes.toolTipText}`}>{roomName}</span>
        </div>
    )
}

export default RoomButton