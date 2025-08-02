// SPDX-FileCopyrightText: 2025 Simon Wendel
// SPDX-License-Identifier: LGPL-3.0-or-later

namespace ValideraFx.Web;

public class ValidationOptions
{
    public bool EnforceValidModelState { get; set; }
    public bool EnforceValidatedTypes { get; set; }
    public bool DontRenderValues { get; set; }
    public bool StrictValidationMode
    {
        get => EnforceValidModelState && EnforceValidatedTypes && DontRenderValues;
        set
        {
            if (!value)
            {
                return;
            }

            EnforceValidModelState = true;
            EnforceValidatedTypes = true;
            DontRenderValues = true;
        }
    }
}