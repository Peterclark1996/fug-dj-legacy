import { useCallback, useEffect, useRef, useState } from "react"
import TagData from "../../Types/TagData"
import classes from "./Tag.module.scss"

type TagProps = {
    availableTags: TagData[],
    colourHex: string,
    onAddConfirmClick: (name: string) => void,
    onAddCancelClick: () => void
}

const KEY_CODE_ENTER = 13
const KEY_CODE_BACKSPACE = 8

const TagInput = ({ availableTags, colourHex, onAddConfirmClick, onAddCancelClick }: TagProps) => {
    const [newTagLabel, setNewTagLabel] = useState("")
    const ref = useRef<HTMLDivElement>(null)

    const tagsToShow = availableTags.filter(tag => tag.name.toLowerCase().includes(newTagLabel.toLowerCase()))

    const isLabelAnAvailableTag = availableTags.find(tag => tag.name.toLowerCase() === newTagLabel.toLowerCase())

    const onKeyDown = (keyId: number) => {
        if (keyId === KEY_CODE_ENTER) {
            if (isLabelAnAvailableTag || newTagLabel.length > 0) {
                onAddConfirmClick(newTagLabel)
            } else {
                onAddCancelClick()
            }
        }

        if (keyId === KEY_CODE_BACKSPACE) {
            if (newTagLabel.length === 0) {
                onAddCancelClick()
            }
        }
    }

    const handleClickOutsideComponent = useCallback(e => {
        if (!ref || !ref.current) return
        if (ref.current.contains(e.target)) return
        onAddCancelClick()
    }, [onAddCancelClick])

    useEffect(() => {
        document.addEventListener("mousedown", handleClickOutsideComponent)
        return () => document.removeEventListener("mousedown", handleClickOutsideComponent)
    }, [handleClickOutsideComponent]);

    return (
        <div className="d-flex position-relative align-items-center flex-column" ref={ref}>
            <div
                className={`d-flex align-items-center px-1 me-1 mb-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
                style={{ "backgroundColor": `#${colourHex}` }}
            >
                <input
                    autoFocus
                    className={`text-light rounded px-1 ${classes.inputField}`}
                    value={newTagLabel}
                    type="text"
                    onChange={event => setNewTagLabel(event.target.value)}
                    style={{ "width": `${newTagLabel.length}ch` }}
                    onKeyDown={event => onKeyDown(event.keyCode)}
                />
            </div>
            <div className="ms-1">
                <div className={`d-block position-absolute rounded px-1 pt-1 ${classes.shadow} ${classes.scrollable} ${classes.options}`}>
                    <div className="d-flex flex-column align-items-center">
                        {
                            tagsToShow.map(tag =>
                                <div
                                    key={tag.id}
                                    className={`d-flex align-items-center px-1 mb-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
                                    role="button"
                                    style={{ "backgroundColor": `#${tag.colourHex}` }}
                                    onMouseDown={() => onAddConfirmClick(tag.name)}
                                >
                                    <span className={classes.tagText}>{tag.name}</span>
                                </div>
                            )
                        }
                        {
                            newTagLabel.length > 0 && !isLabelAnAvailableTag && <>
                                {tagsToShow.length > 0 && <div className="w-100 border mb-1" />}
                                <div
                                    className={`d-flex align-items-center px-1 mb-1 ${classes.rounded} ${classes.shadow} ${classes.smallFont}`}
                                    role="button"
                                    onClick={() => onAddConfirmClick(newTagLabel)}
                                >
                                    <span className={classes.tagText}>{newTagLabel}</span>
                                    <i className="ms-1 text-light-grey fa-solid fa-plus" />
                                </div>
                            </>
                        }
                    </div>
                </div>
            </div>
        </div >
    )
}

export default TagInput