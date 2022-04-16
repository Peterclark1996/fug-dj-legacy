import TagData from "../../Types/TagData"
import classes from "./Tag.module.scss"

type TagProps = {
    label: string,
    onLabelChange: (name: string) => void,
    availableTags: TagData[],
    colourHex: string,
    onAddConfirmClick: () => void,
    onAddCancelClick: () => void
}

const TagInput = ({ label, onLabelChange, availableTags, colourHex, onAddConfirmClick, onAddCancelClick }: TagProps) => {

    const tagsToShow = availableTags.filter(tag => tag.name.toLowerCase().includes(label.toLowerCase()))

    return (
        <div className="d-flex position-relative flex-column">
            <div
                className={`d-flex align-items-center px-1 me-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
                style={{ "backgroundColor": `#${colourHex}` }}
            >
                <input
                    autoFocus
                    className={`text-light rounded ${classes.inputField}`}
                    value={label}
                    type="text"
                    onChange={event => onLabelChange(event.target.value)}
                    style={{ "width": `${label.length}ch` }}
                />
                <i className="ms-1 text-success fa-solid fa-circle-check" role="button" onClick={onAddConfirmClick} />
                <i className="ms-1 text-danger fa-solid fa-circle-xmark" role="button" onClick={onAddCancelClick} />
            </div>
            <div className="ms-1">
                <div className={`d-block position-absolute bg-secondary rounded px-1 pt-1 ${classes.shadow} ${classes.scrollable} ${classes.options}`}>
                    <div className="d-flex flex-column">
                        {
                            tagsToShow.map(tag =>
                                <div
                                    className={`d-flex align-items-center px-1 mb-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
                                    role="button"
                                    style={{ "backgroundColor": `#${tag.colourHex}` }}
                                    onClick={() => onLabelChange(tag.name)}
                                >
                                    <span>{tag.name}</span>
                                </div>

                            )
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}

export default TagInput