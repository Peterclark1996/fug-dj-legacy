import classes from "./Tag.module.scss"

type AddTagProps = {
    onClick: () => void
}

const AddTag = ({ onClick }: AddTagProps) => {
    return (
        <div
            className={`d-flex align-items-center px-1 me-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont} ${classes.addButtonColour}`}
            role="button"
            onClick={onClick}
        >
            <span>Add</span>
            <i className="ms-1 text-light-grey fa-solid fa-plus" />
        </div>
    )
}

export default AddTag